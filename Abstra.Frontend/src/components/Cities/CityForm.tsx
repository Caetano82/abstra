import React, { useEffect, useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  FormHelperText,
} from '@mui/material';
import type { CreateCityRequest, UpdateCityRequest, City, State } from '../../types';

interface CityFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CreateCityRequest | UpdateCityRequest) => void;
  city?: City | null;
  states: State[];
  loading?: boolean;
}

export const CityForm: React.FC<CityFormProps> = ({
  open,
  onClose,
  onSubmit,
  city,
  states,
  loading = false,
}) => {
  const [name, setName] = useState('');
  const [stateId, setStateId] = useState<number>(0);
  const [population, setPopulation] = useState<string>('');
  const [errors, setErrors] = useState<{ name?: string; stateId?: string }>({});

  useEffect(() => {
    if (city) {
      setName(city.name);
      setStateId(city.stateId);
      setPopulation(city.population?.toString() || '');
    } else {
      setName('');
      setStateId(0);
      setPopulation('');
    }
    setErrors({});
  }, [city, open]);

  const validate = () => {
    const newErrors: { name?: string; stateId?: string } = {};
    if (!name.trim()) {
      newErrors.name = 'Name is required';
    }
    if (!stateId || stateId === 0) {
      newErrors.stateId = 'State is required';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = () => {
    if (validate()) {
      const data: CreateCityRequest | UpdateCityRequest = {
        name: name.trim(),
        stateId,
        population: population ? parseInt(population) : undefined,
      };
      onSubmit(data);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{city ? 'Edit City' : 'New City'}</DialogTitle>
      <DialogContent>
        <Box sx={{ pt: 2 }}>
          <TextField
            autoFocus
            margin="dense"
            label="Name"
            fullWidth
            variant="outlined"
            value={name}
            onChange={(e) => setName(e.target.value)}
            error={!!errors.name}
            helperText={errors.name}
            disabled={loading}
          />
          <FormControl fullWidth margin="dense" error={!!errors.stateId}>
            <InputLabel>State</InputLabel>
            <Select
              value={stateId}
              onChange={(e) => setStateId(e.target.value as number)}
              label="State"
              disabled={loading}
            >
              {states.map((state) => (
                <MenuItem key={state.id} value={state.id}>
                  {state.name} ({state.code}) - {state.countryName}
                </MenuItem>
              ))}
            </Select>
            {errors.stateId && <FormHelperText>{errors.stateId}</FormHelperText>}
          </FormControl>
          <TextField
            margin="dense"
            label="Population"
            type="number"
            fullWidth
            variant="outlined"
            value={population}
            onChange={(e) => setPopulation(e.target.value)}
            disabled={loading}
            helperText="Optional"
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={loading}>
          Cancel
        </Button>
        <Button onClick={handleSubmit} variant="contained" disabled={loading}>
          {loading ? 'Saving...' : 'Save'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
