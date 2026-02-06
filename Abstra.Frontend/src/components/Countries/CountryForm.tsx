import React, { useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
} from '@mui/material';
import type { CreateCountryRequest, UpdateCountryRequest, Country } from '../../types';

interface CountryFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CreateCountryRequest | UpdateCountryRequest) => void;
  country?: Country | null;
  loading?: boolean;
}

export const CountryForm: React.FC<CountryFormProps> = ({
  open,
  onClose,
  onSubmit,
  country,
  loading = false,
}) => {
  const [name, setName] = React.useState('');
  const [code, setCode] = React.useState('');
  const [errors, setErrors] = React.useState<{ name?: string; code?: string }>({});

  useEffect(() => {
    if (country) {
      setName(country.name);
      setCode(country.code);
    } else {
      setName('');
      setCode('');
    }
    setErrors({});
  }, [country, open]);

  const validate = () => {
    const newErrors: { name?: string; code?: string } = {};
    if (!name.trim()) {
      newErrors.name = 'Name is required';
    }
    if (!code.trim()) {
      newErrors.code = 'Code is required';
    } else if (code.length !== 3) {
      newErrors.code = 'Code must be 3 characters';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = () => {
    if (validate()) {
      onSubmit({ name: name.trim(), code: code.trim().toUpperCase() });
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{country ? 'Edit Country' : 'New Country'}</DialogTitle>
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
            label="Código (ISO)"
            fullWidth
            variant="outlined"
            value={code}
            onChange={(e) => setCode(e.target.value.toUpperCase())}
            error={!!errors.code}
            helperText={errors.code || 'Ex: USA, BRA'}
            disabled={loading}
            inputProps={{ maxLength: 3 }}
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
