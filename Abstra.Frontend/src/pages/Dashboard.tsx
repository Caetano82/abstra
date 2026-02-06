import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography, Box, CircularProgress } from '@mui/material';
import { Public as PublicIcon, LocationCity as LocationCityIcon, Map as MapIcon } from '@mui/icons-material';
import { countryService } from '../services/countryService';
import { stateService } from '../services/stateService';
import { cityService } from '../services/cityService';
import { MainLayout } from '../components/Layout/MainLayout';

export const Dashboard: React.FC = () => {
  const [countriesCount, setCountriesCount] = useState<number>(0);
  const [statesCount, setStatesCount] = useState<number>(0);
  const [citiesCount, setCitiesCount] = useState<number>(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [countries, states, cities] = await Promise.all([
          countryService.getAll(),
          stateService.getAll(),
          cityService.getAll(),
        ]);
        setCountriesCount(countries.length);
        setStatesCount(states.length);
        setCitiesCount(cities.length);
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <MainLayout>
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
          <CircularProgress />
        </Box>
      </MainLayout>
    );
  }

  return (
    <MainLayout>
      <Typography variant="h4" gutterBottom>
        Dashboard
      </Typography>
      <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
        System overview
      </Typography>

      <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 3 }}>
        <Box sx={{ flex: 1 }}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" mb={2}>
                <PublicIcon sx={{ fontSize: 40, color: 'primary.main', mr: 2 }} />
                <Box>
                  <Typography color="text.secondary" gutterBottom>
                    Total Countries
                  </Typography>
                  <Typography variant="h4">{countriesCount}</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Box>

        <Box sx={{ flex: 1 }}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" mb={2}>
                <MapIcon sx={{ fontSize: 40, color: 'secondary.main', mr: 2 }} />
                <Box>
                  <Typography color="text.secondary" gutterBottom>
                    Total States
                  </Typography>
                  <Typography variant="h4">{statesCount}</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Box>

        <Box sx={{ flex: 1 }}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" mb={2}>
                <LocationCityIcon sx={{ fontSize: 40, color: 'secondary.main', mr: 2 }} />
                <Box>
                  <Typography color="text.secondary" gutterBottom>
                    Total Cities
                  </Typography>
                  <Typography variant="h4">{citiesCount}</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Box>
      </Box>
    </MainLayout>
  );
};
