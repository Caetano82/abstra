import api from './api';
import type { State, CreateStateRequest, UpdateStateRequest } from '../types';

export const stateService = {
  async getAll(): Promise<State[]> {
    const response = await api.get<State[]>('/states');
    return response.data;
  },

  async getByCountryId(countryId: number): Promise<State[]> {
    const response = await api.get<State[]>(`/states/country/${countryId}`);
    return response.data;
  },

  async getById(id: number): Promise<State> {
    const response = await api.get<State>(`/states/${id}`);
    return response.data;
  },

  async create(data: CreateStateRequest): Promise<State> {
    const response = await api.post<State>('/states', data);
    return response.data;
  },

  async update(id: number, data: UpdateStateRequest): Promise<State> {
    const response = await api.put<State>(`/states/${id}`, data);
    return response.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/states/${id}`);
  },
};
