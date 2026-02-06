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
import { CityForm } from '../components/Cities/CityForm';
import { CityCard } from '../components/Cities/CityCard';
import { Notification } from '../components/Notification';
import { cityService } from '../services/cityService';
import { stateService } from '../services/stateService';
import type { City, State, CreateCityRequest, UpdateCityRequest } from '../types';

export const Cities: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [cities, setCities] = useState<City[]>([]);
  const [states, setStates] = useState<State[]>([]);
  const [filteredCities, setFilteredCities] = useState<City[]>([]);
  const [selectedStateFilter, setSelectedStateFilter] = useState<number>(0);
  const [loading, setLoading] = useState(true);
  const [formOpen, setFormOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedCity, setSelectedCity] = useState<City | null>(null);
  const [cityToDelete, setCityToDelete] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [notification, setNotification] = useState<{
    open: boolean;
    message: string;
    severity: 'success' | 'error';
  }>({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    // Check if there's a state filter in the URL
    const stateParam = searchParams.get('state');
    if (stateParam) {
      const stateId = parseInt(stateParam, 10);
      if (!isNaN(stateId)) {
        setSelectedStateFilter(stateId);
      }
    } else {
      setSelectedStateFilter(0);
    }
  }, [searchParams]);

  useEffect(() => {
    if (selectedStateFilter === 0) {
      setFilteredCities(cities);
    } else {
      setFilteredCities(cities.filter((city) => city.stateId === selectedStateFilter));
    }
  }, [selectedStateFilter, cities]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [citiesData, statesData] = await Promise.all([
        cityService.getAll(),
        stateService.getAll(),
      ]);
      setCities(citiesData);
      setFilteredCities(citiesData);
      setStates(statesData);
    } catch (error: any) {
      showNotification('Error loading data', 'error');
    } finally {
      setLoading(false);
    }
  };

  const showNotification = (message: string, severity: 'success' | 'error') => {
    setNotification({ open: true, message, severity });
  };

  const handleOpenForm = (city?: City) => {
    setSelectedCity(city || null);
    setFormOpen(true);
  };

  const handleCloseForm = () => {
    setFormOpen(false);
    setSelectedCity(null);
  };

  const handleSubmit = async (data: CreateCityRequest | UpdateCityRequest) => {
    try {
      setSubmitting(true);
      if (selectedCity) {
        await cityService.update(selectedCity.id, data as UpdateCityRequest);
        showNotification('City updated successfully!', 'success');
      } else {
        await cityService.create(data as CreateCityRequest);
        showNotification('City created successfully!', 'success');
      }
      handleCloseForm();
      loadData();
    } catch (error: any) {
      showNotification(
        error.response?.data?.error || 'Error saving city',
        'error'
      );
    } finally {
      setSubmitting(false);
    }
  };

  const handleDeleteClick = (id: number) => {
    setCityToDelete(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (cityToDelete) {
      try {
        await cityService.delete(cityToDelete);
        showNotification('City deleted successfully!', 'success');
        loadData();
      } catch (error: any) {
        showNotification('Error deleting city', 'error');
      } finally {
        setDeleteDialogOpen(false);
        setCityToDelete(null);
      }
    }
  };

  const handleViewCountry = (countryId?: number) => {
    if (countryId) {
      navigate(`/countries`);
    }
  };

  return (
    <MainLayout>
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap', gap: 2 }}>
        <Typography variant="h4">Cities</Typography>
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap' }}>
          <FormControl sx={{ minWidth: 200 }} size="small">
            <InputLabel>Filter by State</InputLabel>
            <Select
              value={selectedStateFilter}
              onChange={(e) => {
                const stateId = e.target.value as number;
                setSelectedStateFilter(stateId);
                // Update URL
                if (stateId === 0) {
                  setSearchParams({});
                } else {
                  setSearchParams({ state: stateId.toString() });
                }
              }}
              label="Filter by State"
            >
              <MenuItem value={0}>All</MenuItem>
              {states.map((state) => (
                <MenuItem key={state.id} value={state.id}>
                  {state.name} ({state.code}) - {state.countryName}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenForm()}
          >
            New City
          </Button>
        </Box>
      </Box>

      {loading ? (
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
          <CircularProgress />
        </Box>
      ) : filteredCities.length === 0 ? (
        <Box textAlign="center" py={4}>
          <Typography variant="h6" color="text.secondary">
            {selectedStateFilter === 0
              ? 'No cities registered'
              : 'No cities found for this state'}
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenForm()}
            sx={{ mt: 2 }}
          >
            Create First City
          </Button>
        </Box>
      ) : (
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)', md: 'repeat(3, 1fr)' }, gap: 3 }}>
          {filteredCities.map((city) => (
            <CityCard
              key={city.id}
              city={city}
              onEdit={handleOpenForm}
              onDelete={handleDeleteClick}
              onViewCountry={handleViewCountry}
            />
          ))}
        </Box>
      )}

      <CityForm
        open={formOpen}
        onClose={handleCloseForm}
        onSubmit={handleSubmit}
        city={selectedCity}
        states={states}
        loading={submitting}
      />

      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this city? This action cannot be undone.
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
