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
} from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { MainLayout } from '../components/Layout/MainLayout';
import { CountryForm } from '../components/Countries/CountryForm';
import { CountryCard } from '../components/Countries/CountryCard';
import { CountryDetailsDialog } from '../components/Countries/CountryDetailsDialog';
import { Notification } from '../components/Notification';
import { countryService } from '../services/countryService';
import { stateService } from '../services/stateService';
import type { Country, CreateCountryRequest, UpdateCountryRequest } from '../types';

export const Countries: React.FC = () => {
  const navigate = useNavigate();
  const [countries, setCountries] = useState<Country[]>([]);
  const [countriesWithStates, setCountriesWithStates] = useState<Map<number, number>>(new Map());
  const [loading, setLoading] = useState(true);
  const [formOpen, setFormOpen] = useState(false);
  const [detailsDialogOpen, setDetailsDialogOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedCountry, setSelectedCountry] = useState<Country | null>(null);
  const [countryToDelete, setCountryToDelete] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [notification, setNotification] = useState<{
    open: boolean;
    message: string;
    severity: 'success' | 'error';
  }>({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    loadCountries();
  }, []);

  const loadCountries = async () => {
    try {
      setLoading(true);
      const [countriesData, statesData] = await Promise.all([
        countryService.getAll(),
        stateService.getAll(),
      ]);
      setCountries(countriesData);
      
      // Count states per country
      const statesCountMap = new Map<number, number>();
      statesData.forEach(state => {
        const count = statesCountMap.get(state.countryId) || 0;
        statesCountMap.set(state.countryId, count + 1);
      });
      setCountriesWithStates(statesCountMap);
    } catch (error: any) {
      showNotification('Error loading countries', 'error');
    } finally {
      setLoading(false);
    }
  };

  const handleViewStates = (countryId: number) => {
    navigate(`/states?country=${countryId}`);
  };

  const handleViewDetails = (country: Country) => {
    setSelectedCountry(country);
    setDetailsDialogOpen(true);
  };

  const showNotification = (message: string, severity: 'success' | 'error') => {
    setNotification({ open: true, message, severity });
  };

  const handleOpenForm = (country?: Country) => {
    setSelectedCountry(country || null);
    setFormOpen(true);
  };

  const handleCloseForm = () => {
    setFormOpen(false);
    setSelectedCountry(null);
  };

  const handleSubmit = async (data: CreateCountryRequest | UpdateCountryRequest) => {
    try {
      setSubmitting(true);
      if (selectedCountry) {
        await countryService.update(selectedCountry.id, data as UpdateCountryRequest);
        showNotification('Country updated successfully!', 'success');
      } else {
        await countryService.create(data as CreateCountryRequest);
        showNotification('Country created successfully!', 'success');
      }
      handleCloseForm();
      loadCountries();
    } catch (error: any) {
      showNotification(
        error.response?.data?.error || 'Error saving country',
        'error'
      );
    } finally {
      setSubmitting(false);
    }
  };

  const handleDeleteClick = (id: number) => {
    setCountryToDelete(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (countryToDelete) {
      try {
        await countryService.delete(countryToDelete);
        showNotification('Country deleted successfully!', 'success');
        loadCountries();
      } catch (error: any) {
        showNotification('Error deleting country', 'error');
      } finally {
        setDeleteDialogOpen(false);
        setCountryToDelete(null);
      }
    }
  };

  return (
    <MainLayout>
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4">Countries</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpenForm()}
        >
          New Country
        </Button>
      </Box>

      {loading ? (
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
          <CircularProgress />
        </Box>
      ) : countries.length === 0 ? (
        <Box textAlign="center" py={4}>
          <Typography variant="h6" color="text.secondary">
            No countries registered
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenForm()}
            sx={{ mt: 2 }}
          >
            Create First Country
          </Button>
        </Box>
      ) : (
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)', md: 'repeat(3, 1fr)' }, gap: 3 }}>
          {countries.map((country) => (
            <CountryCard
              key={country.id}
              country={country}
              statesCount={countriesWithStates.get(country.id) || 0}
              onEdit={handleOpenForm}
              onDelete={handleDeleteClick}
              onViewStates={handleViewStates}
              onViewDetails={handleViewDetails}
            />
          ))}
        </Box>
      )}

      <CountryForm
        open={formOpen}
        onClose={handleCloseForm}
        onSubmit={handleSubmit}
        country={selectedCountry}
        loading={submitting}
      />

      <CountryDetailsDialog
        open={detailsDialogOpen}
        onClose={() => {
          setDetailsDialogOpen(false);
          setSelectedCountry(null);
        }}
        country={selectedCountry}
      />

      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this country? This action cannot be undone.
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
