import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  Typography,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  DialogContentText,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { MainLayout } from '../components/Layout/MainLayout';
import { StateForm } from '../components/States/StateForm';
import { StateCard } from '../components/States/StateCard';
import { Notification } from '../components/Notification';
import { stateService } from '../services/stateService';
import { countryService } from '../services/countryService';
import { cityService } from '../services/cityService';
import type { State, Country, CreateStateRequest, UpdateStateRequest } from '../types';

export const States: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [states, setStates] = useState<State[]>([]);
  const [countries, setCountries] = useState<Country[]>([]);
  const [filteredStates, setFilteredStates] = useState<State[]>([]);
  const [statesWithCities, setStatesWithCities] = useState<Map<number, number>>(new Map());
  const [selectedCountryFilter, setSelectedCountryFilter] = useState<number>(0);
  const [loading, setLoading] = useState(true);
  const [formOpen, setFormOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedState, setSelectedState] = useState<State | null>(null);
  const [stateToDelete, setStateToDelete] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [notification, setNotification] = useState<{
    open: boolean;
    message: string;
    severity: 'success' | 'error';
  }>({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    loadData();
    // Check if there's a country filter in the URL
    const countryParam = searchParams.get('country');
    if (countryParam) {
      const countryId = parseInt(countryParam, 10);
      if (!isNaN(countryId)) {
        setSelectedCountryFilter(countryId);
      }
    }
  }, []);

  useEffect(() => {
    if (selectedCountryFilter === 0) {
      setFilteredStates(states);
    } else {
      setFilteredStates(states.filter((state) => state.countryId === selectedCountryFilter));
    }
  }, [selectedCountryFilter, states]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [statesData, countriesData, citiesData] = await Promise.all([
        stateService.getAll(),
        countryService.getAll(),
        cityService.getAll(),
      ]);
      setStates(statesData);
      setFilteredStates(statesData);
      setCountries(countriesData);

      // Count cities per state
      const citiesCountMap = new Map<number, number>();
      citiesData.forEach(city => {
        const count = citiesCountMap.get(city.stateId) || 0;
        citiesCountMap.set(city.stateId, count + 1);
      });
      setStatesWithCities(citiesCountMap);
    } catch (error: any) {
      showNotification('Error loading data', 'error');
    } finally {
      setLoading(false);
    }
  };

  const showNotification = (message: string, severity: 'success' | 'error') => {
    setNotification({ open: true, message, severity });
  };

  const handleOpenForm = (state?: State) => {
    setSelectedState(state || null);
    setFormOpen(true);
  };

  const handleCloseForm = () => {
    setFormOpen(false);
    setSelectedState(null);
  };

  const handleSubmit = async (data: CreateStateRequest | UpdateStateRequest) => {
    try {
      setSubmitting(true);
      if (selectedState) {
        await stateService.update(selectedState.id, data as UpdateStateRequest);
        showNotification('State updated successfully!', 'success');
      } else {
        await stateService.create(data as CreateStateRequest);
        showNotification('State created successfully!', 'success');
      }
      handleCloseForm();
      loadData();
    } catch (error: any) {
      showNotification(
        error.response?.data?.error || 'Error saving state',
        'error'
      );
    } finally {
      setSubmitting(false);
    }
  };

  const handleDeleteClick = (id: number) => {
    setStateToDelete(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (stateToDelete) {
      try {
        await stateService.delete(stateToDelete);
        showNotification('State deleted successfully!', 'success');
        loadData();
      } catch (error: any) {
        showNotification('Error deleting state', 'error');
      } finally {
        setDeleteDialogOpen(false);
        setStateToDelete(null);
      }
    }
  };

  const handleViewCities = (stateId: number) => {
    navigate(`/cities?state=${stateId}`);
  };

  return (
    <MainLayout>
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap', gap: 2 }}>
        <Typography variant="h4">States</Typography>
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap' }}>
          <FormControl sx={{ minWidth: 200 }} size="small">
            <InputLabel>Filter by Country</InputLabel>
            <Select
              value={selectedCountryFilter}
              onChange={(e) => {
                const countryId = e.target.value as number;
                setSelectedCountryFilter(countryId);
                if (countryId === 0) {
                  setSearchParams({});
                } else {
                  setSearchParams({ country: countryId.toString() });
                }
              }}
              label="Filter by Country"
            >
              <MenuItem value={0}>All</MenuItem>
              {countries.map((country) => (
                <MenuItem key={country.id} value={country.id}>
                  {country.name} ({country.code})
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenForm()}
          >
            New State
          </Button>
        </Box>
      </Box>

      {loading ? (
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
          <CircularProgress />
        </Box>
      ) : filteredStates.length === 0 ? (
        <Box textAlign="center" py={4}>
          <Typography variant="h6" color="text.secondary">
            {selectedCountryFilter === 0
              ? 'No states registered'
              : 'No states found for this country'}
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenForm()}
            sx={{ mt: 2 }}
          >
            Create First State
          </Button>
        </Box>
      ) : (
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)', md: 'repeat(3, 1fr)' }, gap: 3 }}>
          {filteredStates.map((state) => (
            <StateCard
              key={state.id}
              state={state}
              citiesCount={statesWithCities.get(state.id) || 0}
              onEdit={handleOpenForm}
              onDelete={handleDeleteClick}
              onViewCities={handleViewCities}
            />
          ))}
        </Box>
      )}

      <StateForm
        open={formOpen}
        onClose={handleCloseForm}
        onSubmit={handleSubmit}
        state={selectedState}
        countries={countries}
        loading={submitting}
      />

      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this state? All associated cities will also be deleted. This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Cancel</Button>
          <Button onClick={handleDeleteConfirm} color="error" variant="contained">
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      <Notification
        open={notification.open}
        message={notification.message}
        severity={notification.severity}
        onClose={() => setNotification({ ...notification, open: false })}
      />
    </MainLayout>
  );
};
