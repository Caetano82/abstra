import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  IconButton,
  Box,
  Chip,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Public as PublicIcon } from '@mui/icons-material';
import type { City } from '../../types';

interface CityCardProps {
  city: City;
  onEdit: (city: City) => void;
  onDelete: (id: number) => void;
  onViewCountry?: (countryId: number) => void;
}

export const CityCard: React.FC<CityCardProps> = ({ city, onEdit, onDelete, onViewCountry }) => {
  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        <Box display="flex" justifyContent="space-between" alignItems="start" mb={2}>
          <Box sx={{ flex: 1 }}>
            <Typography variant="h6" component="div">
              {city.name}
            </Typography>
            <Box sx={{ mt: 1, display: 'flex', alignItems: 'center', gap: 0.5, flexWrap: 'wrap' }}>
              {city.stateName && (
                <Chip 
                  label={city.stateName} 
                  size="small" 
                  color="secondary"
                />
              )}
              {city.countryName && (
                <>
                  <PublicIcon sx={{ fontSize: 14, color: 'text.secondary' }} />
                  <Chip 
                    label={city.countryName} 
                    size="small" 
                    color="primary"
                    sx={{ cursor: onViewCountry ? 'pointer' : 'default' }}
                    onClick={onViewCountry && city.countryId ? () => onViewCountry(city.countryId!) : undefined}
                  />
                </>
              )}
            </Box>
          </Box>
          <Box>
            <IconButton
              size="small"
              color="primary"
              onClick={() => onEdit(city)}
              aria-label="edit"
            >
              <EditIcon />
            </IconButton>
            <IconButton
              size="small"
              color="error"
              onClick={() => onDelete(city.id)}
              aria-label="delete"
            >
              <DeleteIcon />
            </IconButton>
          </Box>
        </Box>
        {city.population && (
          <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
            Population: {city.population.toLocaleString('en-US')}
          </Typography>
        )}
        <Typography variant="caption" color="text.secondary">
          Created at: {new Date(city.createdAt).toLocaleDateString('en-US')}
        </Typography>
      </CardContent>
    </Card>
  );
};
