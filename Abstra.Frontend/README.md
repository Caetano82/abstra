# Abstra Frontend - React Application

Modern and responsive React application to manage Countries, States, and Cities through the REST API.

## Technologies

- React 18 with TypeScript
- Vite - Build tool
- Material-UI (MUI) - UI components
- React Router v6 - Routing
- Axios - HTTP client
- Context API - State management

## Installation

```bash
npm install
```

## Configuration

Create a `.env` file in the project root with the API URL:

```
VITE_API_BASE_URL=https://localhost:7246/api
```

**Note:** The `.env` file is already configured with the default API URL.

## Development

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## Production Build

```bash
npm run build
```

## Project Structure

```
src/
├── components/      # Reusable components
│   ├── Countries/  # Country-related components
│   ├── States/     # State-related components
│   ├── Cities/     # City-related components
│   ├── Auth/       # Authentication components
│   └── Layout/     # Layout components
├── contexts/       # Context API (AuthContext)
├── pages/          # Pages/routes
│   ├── Login.tsx
│   ├── Dashboard.tsx
│   ├── Countries.tsx
│   ├── States.tsx
│   └── Cities.tsx
├── services/       # API services
├── hooks/          # Custom hooks
├── types/          # TypeScript types
└── utils/          # Utilities
```

## User Interface

- **Countries Page**: View all countries with state count. Click "View States" to see states for a country, or "Details" to view states in a dialog.
- **States Page**: View all states with city count. Filter by country. Click "View Cities" to see cities for a state.
- **Cities Page**: View all cities. Filter by state.

## Features

- ✅ JWT Authentication
- ✅ Dashboard with statistics
- ✅ Complete CRUD for Countries (with States view)
- ✅ Complete CRUD for States (with Cities view)
- ✅ Complete CRUD for Cities
- ✅ Three-level hierarchy: Country → State → City
- ✅ Responsive layout (mobile, tablet, desktop)
- ✅ Form validation
- ✅ Notifications (Snackbar)
- ✅ Error handling
- ✅ Loading states
- ✅ Navigation between related entities

## Default Credentials

- Username: `admin`
- Password: `admin123`

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Create production build
- `npm run preview` - Preview production build
