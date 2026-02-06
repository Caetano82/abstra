import api from './api';
import type { Country, CreateCountryRequest, UpdateCountryRequest } from '../types';

export const countryService = {
  async getAll(): Promise<Country[]> {
    const response = await api.get<Country[]>('/countries');
    return response.data;
  },

  async getById(id: number): Promise<Country> {
    const response = await api.get<Country>(`/countries/${id}`);
    return response.data;
  },

  async create(data: CreateCountryRequest): Promise<Country> {
    const response = await api.post<Country>('/countries', data);
    return response.data;
  },

  async update(id: number, data: UpdateCountryRequest): Promise<Country> {
    const response = await api.put<Country>(`/countries/${id}`, data);
    return response.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/countries/${id}`);
  },
};
