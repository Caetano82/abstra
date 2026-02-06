import React, { useState } from 'react';
import { Box, Toolbar, IconButton } from '@mui/material';
import { Menu as MenuIcon } from '@mui/icons-material';
import { Header } from './Header';
import { Sidebar } from './Sidebar';

interface MainLayoutProps {
  children: React.ReactNode;
}

export const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <Header />
      <Box sx={{ display: 'flex', flexGrow: 1 }}>
        <Box
          component="nav"
          sx={{ width: { sm: 240 }, flexShrink: { sm: 0 } }}
        >
          <Sidebar mobileOpen={mobileOpen} onMobileToggle={handleDrawerToggle} />
        </Box>
        <Box
          component="main"
          sx={{
            flexGrow: 1,
            p: 3,
            width: { sm: `calc(100% - 240px)` },
            backgroundColor: '#f5f5f5',
          }}
        >
          <Toolbar />
          <Box
            sx={{
              display: { xs: 'block', sm: 'none' },
              position: 'fixed',
              top: 64,
              left: 0,
              zIndex: 1200,
            }}
          >
            <IconButton
              color="inherit"
              aria-label="open drawer"
              edge="start"
              onClick={handleDrawerToggle}
              sx={{ ml: 1, mt: 1, backgroundColor: 'rgba(0,0,0,0.1)' }}
            >
              <MenuIcon />
            </IconButton>
          </Box>
          {children}
        </Box>
      </Box>
    </Box>
  );
};
