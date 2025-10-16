import React from 'react'
import { type Pessoa } from '../models/pessoa'

interface Props {
  pessoa: Pessoa;
  onEdit?: (p: Pessoa) => void;
  onDelete?: (id: string) => void;
}

export const Card: React.FC<Props> = ({ pessoa, onEdit, onDelete }) => (
  <div className="bg-white shadow-md rounded p-4 flex justify-between items-center hover:shadow-lg transition">
    <div>
      <h2 className="font-bold text-lg">{pessoa.nome}</h2>
      <p className="text-gray-500 text-sm">{pessoa.dataNascimento}</p>
    </div>
    <div className="flex gap-2">
      {onEdit && <button className="text-blue-500 hover:text-blue-700" onClick={() => onEdit(pessoa)}>✏️</button>}
      {onDelete && <button className="text-red-500 hover:text-red-700" onClick={() => onDelete(pessoa.id)}>🗑️</button>}
    </div>
  </div>
);
