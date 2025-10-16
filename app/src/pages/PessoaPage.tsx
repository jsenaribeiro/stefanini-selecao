import React, { useState } from 'react'
import { usePessoas } from '../hooks/usePessoa'
import type { Pessoa } from '../models/pessoa'

const PessoasPage: React.FC = () => {
  const { data: pessoas, isLoading, isError, create, update, remove } = usePessoas();
  const [editing, setEditing] = useState<Pessoa | null>(null);
  const [formVisible, setFormVisible] = useState(false);
  const [nome, setNome] = useState('');
  const [dataNascimento, setDataNascimento] = useState('');

  const handleSave = () => {
    if (editing) {
      update.mutate({ id: editing.id, pessoa: { nome, dataNascimento } });
    } else {
      create.mutate({ nome, dataNascimento });
    }
    setFormVisible(false);
    setEditing(null);
    setNome('');
    setDataNascimento('');
  };

  if (isLoading) return <div>Carregando...</div>;
  if (isError) return <div>Erro ao carregar pessoas</div>;

  return (
    <div className="max-w-xl mx-auto mt-8 flex flex-col gap-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Pessoas</h1>
        <button
          onClick={() => setFormVisible(true)}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
        >
          ➕ Nova
        </button>
      </div>

      {formVisible && (
        <div className="bg-white p-4 shadow-md rounded flex flex-col gap-3">
          <input
            className="border p-2 rounded"
            placeholder="Nome"
            value={nome}
            onChange={e => setNome(e.target.value)}
          />
          <input
            type="date"
            className="border p-2 rounded"
            value={dataNascimento}
            onChange={e => setDataNascimento(e.target.value)}
          />
          <div className="flex gap-2 justify-end">
            <button
              onClick={handleSave}
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
            >
              {editing ? 'Atualizar' : 'Criar'}
            </button>
            <button
              onClick={() => { setFormVisible(false); setEditing(null); }}
              className="bg-gray-300 px-4 py-2 rounded hover:bg-gray-400"
            >
              Cancelar
            </button>
          </div>
        </div>
      )}

      <div className="flex flex-col gap-2">
        {pessoas?.map(p => (
          <div
            key={p.id}
            className="bg-white shadow-md rounded p-4 flex justify-between items-center hover:shadow-lg transition"
          >
            <div>
              <h2 className="font-bold text-lg">{p.nome}</h2>
              <p className="text-gray-500 text-sm">{p.dataNascimento}</p>
            </div>
            <div className="flex gap-2">
              <button
                className="text-blue-500 hover:text-blue-700"
                onClick={() => {
                  setEditing(p);
                  setNome(p.nome);
                  setDataNascimento(p.dataNascimento);
                  setFormVisible(true);
                }}
              >
                ✏️
              </button>
              <button className="text-red-500 hover:text-red-700" onClick={() => remove.mutate(p.id)}>
                🗑️
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default PessoasPage;