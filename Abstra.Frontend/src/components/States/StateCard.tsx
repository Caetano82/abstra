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
import { Edit as EditIcon, Delete as DeleteIcon, LocationCity as LocationCityIcon } from '@mui/icons-material';
import type { State } from '../../types';

interface StateCardProps {
  state: State;
  citiesCount?: number;
  onEdit: (state: State) => void;
  onDelete: (id: number) => void;
  onViewCities?: (stateId: number) => void;
}

export const StateCard: React.FC<StateCardProps> = ({
  state,
  citiesCount,
  onEdit,
  onDelete,
  onViewCities,
}) => {
  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        <Box display="flex" justifyContent="space-between" alignItems="start" mb={2}>
          <Box>
            <Typography variant="h6" component="div">
              {state.name}
            </Typography>
            <Box sx={{ mt: 1, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
              <Chip label={state.code} size="small" color="primary" />
              {state.countryName && (
                <Chip label={state.countryName} size="small" color="secondary" />
              )}
            </Box>
          </Box>
          <Box>
            <IconButton
              size="small"
              color="primary"
              onClick={() => onEdit(state)}
              aria-label="edit"
            >
              <EditIcon />
            </IconButton>
            <IconButton
              size="small"
              color="error"
              onClick={() => onDelete(state.id)}
              aria-label="delete"
            >
              <DeleteIcon />
            </IconButton>
          </Box>
        </Box>
        
        {citiesCount !== undefined && (
          <Box sx={{ mb: 2 }}>
            <Typography variant="body2" color="text.secondary">
              <LocationCityIcon sx={{ fontSize: 16, verticalAlign: 'middle', mr: 0.5 }} />
              {citiesCount} {citiesCount === 1 ? 'city' : 'cities'}
            </Typography>
          </Box>
        )}

        {onViewCities && (
          <Button
            size="small"
            variant="outlined"
            startIcon={<LocationCityIcon />}
            onClick={() => onViewCities(state.id)}
            fullWidth
            sx={{ mb: 1 }}
          >
            View Cities
          </Button>
        )}

        <Typography variant="caption" color="text.secondary">
          Created at: {new Date(state.createdAt).toLocaleDateString('en-US')}
        </Typography>
      </CardContent>
    </Card>
  );
};
