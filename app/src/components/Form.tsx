import React, { useState } from 'react'
import { type Pessoa } from '../models/pessoa'

interface Props {
  pessoa?: Pessoa;
  onSave: (p: Omit<Pessoa, 'id'> | Partial<Pessoa>) => void;
  onClose?: () => void;
}

export const Form: React.FC<Props> = ({ pessoa, onSave, onClose }) => {
  const [nome, setNome] = useState(pessoa?.nome || '');
  const [data, setData] = useState(pessoa?.dataNascimento || '');

  const submit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave({ nome, dataNascimento: data });
    onClose?.();
  };

  return (
    <form className="bg-white p-4 shadow-md rounded flex flex-col gap-3" onSubmit={submit}>
      <input value={nome} onChange={e => setNome(e.target.value)} placeholder="Nome" required className="border p-2 rounded"/>
      <input type="date" value={data} onChange={e => setData(e.target.value)} required className="border p-2 rounded"/>
      <div className="flex justify-end gap-2">
        <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">{pessoa ? 'Atualizar' : 'Criar'}</button>
        {onClose && <button type="button" onClick={onClose} className="bg-gray-300 px-4 py-2 rounded hover:bg-gray-400">Cancelar</button>}
      </div>
    </form>
  );
};
