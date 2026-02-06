import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  IconButton,
  Box,
  Chip,
  Button,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Map as MapIcon } from '@mui/icons-material';
import type { Country } from '../../types';

interface CountryCardProps {
  country: Country;
  statesCount?: number;
  onEdit: (country: Country) => void;
  onDelete: (id: number) => void;
  onViewStates?: (countryId: number) => void;
  onViewDetails?: (country: Country) => void;
}

export const CountryCard: React.FC<CountryCardProps> = ({
  country,
  statesCount,
  onEdit,
  onDelete,
  onViewStates,
  onViewDetails,
}) => {
  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        <Box display="flex" justifyContent="space-between" alignItems="start" mb={2}>
          <Box>
            <Typography variant="h6" component="div">
              {country.name}
            </Typography>
            <Chip label={country.code} size="small" color="primary" sx={{ mt: 1 }} />
          </Box>
          <Box>
            <IconButton
              size="small"
              color="primary"
              onClick={() => onEdit(country)}
              aria-label="edit"
            >
              <EditIcon />
            </IconButton>
            <IconButton
              size="small"
              color="error"
              onClick={() => onDelete(country.id)}
              aria-label="delete"
            >
              <DeleteIcon />
            </IconButton>
          </Box>
        </Box>
        
        {statesCount !== undefined && (
          <Box sx={{ mb: 2 }}>
            <Typography variant="body2" color="text.secondary">
              <MapIcon sx={{ fontSize: 16, verticalAlign: 'middle', mr: 0.5 }} />
              {statesCount} {statesCount === 1 ? 'state' : 'states'}
            </Typography>
          </Box>
        )}

        <Box sx={{ display: 'flex', gap: 1, mb: 1 }}>
          {onViewDetails && (
            <Button
              size="small"
              variant="outlined"
              onClick={() => onViewDetails(country)}
              fullWidth
            >
              Details
            </Button>
          )}
          {onViewStates && (
            <Button
              size="small"
              variant="contained"
              startIcon={<MapIcon />}
              onClick={() => onViewStates(country.id)}
              fullWidth
            >
              View States
            </Button>
          )}
        </Box>

        <Typography variant="caption" color="text.secondary">
          Created at: {new Date(country.createdAt).toLocaleDateString('en-US')}
        </Typography>
      </CardContent>
    </Card>
  );
};
