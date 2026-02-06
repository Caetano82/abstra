import React, { useEffect, useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Box,
  Chip,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
  Divider,
} from '@mui/material';
import { Map as MapIcon } from '@mui/icons-material';
import type { Country, State } from '../../types';
import { stateService } from '../../services/stateService';

interface CountryDetailsDialogProps {
  open: boolean;
  onClose: () => void;
  country: Country | null;
}

export const CountryDetailsDialog: React.FC<CountryDetailsDialogProps> = ({
  open,
  onClose,
  country,
}) => {
  const [states, setStates] = useState<State[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (open && country) {
      loadStates();
    } else {
      setStates([]);
    }
  }, [open, country]);

  const loadStates = async () => {
    if (!country) return;
    
    try {
      setLoading(true);
      const data = await stateService.getByCountryId(country.id);
      setStates(data);
    } catch (error) {
      console.error('Error loading states:', error);
    } finally {
      setLoading(false);
    }
  };

  if (!country) return null;

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>
        <Box display="flex" alignItems="center" gap={2}>
          <Typography variant="h5">{country.name}</Typography>
          <Chip label={country.code} color="primary" />
        </Box>
      </DialogTitle>
      <DialogContent>
        <Box sx={{ mb: 3 }}>
          <Typography variant="body2" color="text.secondary">
            Created at: {new Date(country.createdAt).toLocaleDateString('en-US')}
          </Typography>
        </Box>

        <Divider sx={{ my: 2 }} />

        <Box>
          <Typography variant="h6" gutterBottom>
            <MapIcon sx={{ verticalAlign: 'middle', mr: 1 }} />
            States ({states.length})
          </Typography>

          {loading ? (
            <Box display="flex" justifyContent="center" py={4}>
              <CircularProgress />
            </Box>
          ) : states.length === 0 ? (
            <Typography variant="body2" color="text.secondary" sx={{ py: 2 }}>
              No states registered for this country.
            </Typography>
          ) : (
            <List>
              {states.map((state, index) => (
                <React.Fragment key={state.id}>
                  <ListItem>
                    <ListItemText
                      primary={state.name}
                      secondary={
                        <Box>
                          <Typography variant="caption" display="block">
                            Code: {state.code}
                          </Typography>
                          <Typography variant="caption" color="text.secondary">
                            Created at: {new Date(state.createdAt).toLocaleDateString('en-US')}
                          </Typography>
                        </Box>
                      }
                    />
                  </ListItem>
                  {index < states.length - 1 && <Divider />}
                </React.Fragment>
              ))}
            </List>
          )}
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
};
