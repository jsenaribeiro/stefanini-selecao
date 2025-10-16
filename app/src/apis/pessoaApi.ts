import axios from 'axios';
import { type Pessoa } from '../models/pessoa';

const url = import.meta.env.VITE_API_BASE_URL || 'http://localhost:3000'

const restApi = axios.create({ baseURL: url  });

export const pessoaApi = {
  search: (): Promise<Pessoa[]> => restApi.get('/pessoas').then(res => res.data),
  
  create: (p: Omit<Pessoa, 'id'>) => restApi.post('/pessoas', p).then(res => res.data),
  
  update: (id: string, p: Partial<Pessoa>) => restApi.put(`/pessoas/${id}`, p).then(res => res.data),
  
  delete: (id: string) => restApi.delete(`/pessoas/${id}`),
};
