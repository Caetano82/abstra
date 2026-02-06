import React from 'react';
import {
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Divider,
} from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import {
  Dashboard as DashboardIcon,
  Public as PublicIcon,
  LocationCity as LocationCityIcon,
  Map as MapIcon,
} from '@mui/icons-material';

const drawerWidth = 240;

interface SidebarProps {
  mobileOpen: boolean;
  onMobileToggle: () => void;
}

export const Sidebar: React.FC<SidebarProps> = ({ mobileOpen, onMobileToggle }) => {
  const navigate = useNavigate();
  const location = useLocation();

  const menuItems = [
    { text: 'Dashboard', icon: <DashboardIcon />, path: '/dashboard' },
    { text: 'Countries', icon: <PublicIcon />, path: '/countries' },
    { text: 'States', icon: <MapIcon />, path: '/states' },
    { text: 'Cities', icon: <LocationCityIcon />, path: '/cities' },
  ];

  const handleNavigation = (path: string) => {
    navigate(path);
    onMobileToggle();
  };

  const drawer = (
    <div>
      <Toolbar />
      <Divider />
      <List>
        {menuItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton
              selected={location.pathname === item.path}
              onClick={() => handleNavigation(item.path)}
            >
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </div>
  );

  return (
    <>
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={onMobileToggle}
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: 'block', sm: 'none' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
        }}
      >
        {drawer}
      </Drawer>
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: 'none', sm: 'block' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
        }}
        open
      >
        {drawer}
      </Drawer>
    </>
  );
};
