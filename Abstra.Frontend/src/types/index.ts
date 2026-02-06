export interface Country {
  id: number;
  name: string;
  code: string;
  createdAt: string;
}

export interface State {
  id: number;
  name: string;
  code: string;
  countryId: number;
  countryName?: string;
  createdAt: string;
}

export interface City {
  id: number;
  name: string;
  stateId: number;
  stateName?: string;
  countryId?: number;
  countryName?: string;
  population?: number;
  createdAt: string;
}

export interface CreateCountryRequest {
  name: string;
  code: string;
}

export interface UpdateCountryRequest {
  name: string;
  code: string;
}

export interface CreateStateRequest {
  name: string;
  code: string;
  countryId: number;
}

export interface UpdateStateRequest {
  name: string;
  code: string;
  countryId: number;
}

export interface CreateCityRequest {
  name: string;
  stateId: number;
  population?: number;
}

export interface UpdateCityRequest {
  name: string;
  stateId: number;
  population?: number;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
}
