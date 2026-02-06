import api from './api';
import type { City, CreateCityRequest, UpdateCityRequest } from '../types';

export const cityService = {
  async getAll(): Promise<City[]> {
    const response = await api.get<City[]>('/cities');
    return response.data;
  },

  async getByCountryId(countryId: number): Promise<City[]> {
    const response = await api.get<City[]>(`/cities/country/${countryId}`);
    return response.data;
  },

  async getByStateId(stateId: number): Promise<City[]> {
    const response = await api.get<City[]>(`/cities/state/${stateId}`);
    return response.data;
  },

  async getById(id: number): Promise<City> {
    const response = await api.get<City>(`/cities/${id}`);
    return response.data;
  },

  async create(data: CreateCityRequest): Promise<City> {
    const response = await api.post<City>('/cities', data);
    return response.data;
  },

  async update(id: number, data: UpdateCityRequest): Promise<City> {
    const response = await api.put<City>(`/cities/${id}`, data);
    return response.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/cities/${id}`);
  },
};
