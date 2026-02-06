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
import type { CreateStateRequest, UpdateStateRequest, State, Country } from '../../types';

interface StateFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CreateStateRequest | UpdateStateRequest) => void;
  state?: State | null;
  countries: Country[];
  loading?: boolean;
}

export const StateForm: React.FC<StateFormProps> = ({
  open,
  onClose,
  onSubmit,
  state,
  countries,
  loading = false,
}) => {
  const [name, setName] = useState('');
  const [code, setCode] = useState('');
  const [countryId, setCountryId] = useState<number>(0);
  const [errors, setErrors] = useState<{ name?: string; code?: string; countryId?: string }>({});

  useEffect(() => {
    if (state) {
      setName(state.name);
      setCode(state.code);
      setCountryId(state.countryId);
    } else {
      setName('');
      setCode('');
      setCountryId(0);
    }
    setErrors({});
  }, [state, open]);

  const validate = () => {
    const newErrors: { name?: string; code?: string; countryId?: string } = {};
    if (!name.trim()) {
      newErrors.name = 'Name is required';
    }
    if (!code.trim()) {
      newErrors.code = 'Code is required';
    }
    if (!countryId || countryId === 0) {
      newErrors.countryId = 'Country is required';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = () => {
    if (validate()) {
      onSubmit({ name: name.trim(), code: code.trim().toUpperCase(), countryId });
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{state ? 'Edit State' : 'New State'}</DialogTitle>
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
          <TextField
            margin="dense"
            label="Código"
            fullWidth
            variant="outlined"
            value={code}
            onChange={(e) => setCode(e.target.value.toUpperCase())}
            error={!!errors.code}
            helperText={errors.code || 'Ex: NY, CA, TX'}
            disabled={loading}
            inputProps={{ maxLength: 10 }}
          />
          <FormControl fullWidth margin="dense" error={!!errors.countryId}>
            <InputLabel>Country</InputLabel>
            <Select
              value={countryId}
              onChange={(e) => setCountryId(e.target.value as number)}
              label="Country"
              disabled={loading}
            >
              {countries.map((country) => (
                <MenuItem key={country.id} value={country.id}>
                  {country.name} ({country.code})
                </MenuItem>
              ))}
            </Select>
            {errors.countryId && <FormHelperText>{errors.countryId}</FormHelperText>}
          </FormControl>
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
