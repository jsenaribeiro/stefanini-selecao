import React, { useState } from 'react';
import { useQuery, useMutation, QueryClient, QueryClientProvider } from '@tanstack/react-query';

// --- MOCKS para fins de Demonstração e Funcionalidade (Substitua pelos seus arquivos reais) ---

// 1. Mock do Tipo Pessoa (Pessoa Model)
/**
 * @typedef {object} Pessoa
 * @property {string} id
 * @property {string} nome
 * @property {string} dataNascimento
 */

// 2. Mock dos Dados Iniciais
const initialPessoas = [
  { id: "1", nome: "Ana Luiza", dataNascimento: "1995-10-20" },
  { id: "2", nome: "Rafael Souza", dataNascimento: "1988-03-15" },
  { id: "3", nome: "Clara Mendes", dataNascimento: "2001-07-01" },
];

let globalPessoas = initialPessoas;
let nextId = 4;

// 3. Mock do Hook usePessoas (Simula as operações CRUD com React Query)
const queryClient = new QueryClient();

const usePessoas = () => {
  // Simulação da query (GET)
  const query = useQuery({
    queryKey: ['pessoas'],
    queryFn: () => new Promise(resolve => {
      setTimeout(() => resolve(globalPessoas), 500); // Simula atraso
    }),
  });

  // Simulação de criação (POST)
  const createMutation = useMutation({
    mutationFn: (novaPessoa) => new Promise(resolve => {
      setTimeout(() => {
        const newPessoa = { ...novaPessoa, id: String(nextId++) };
        globalPessoas.push(newPessoa);
        resolve(newPessoa);
      }, 500);
    }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pessoas'] });
    },
  });

  // Simulação de atualização (PUT/PATCH)
  const updateMutation = useMutation({
    mutationFn: (updateData) => new Promise(resolve => {
      setTimeout(() => {
        globalPessoas = globalPessoas.map(p => 
          p.id === updateData.id ? { ...p, ...updateData.pessoa } : p
        );
        resolve(updateData.pessoa);
      }, 500);
    }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pessoas'] });
    },
  });

  // Simulação de remoção (DELETE)
  const removeMutation = useMutation({
    // Corrigido para retornar void, conforme sugerido anteriormente
    mutationFn: (id) => new Promise(resolve => {
      setTimeout(() => {
        globalPessoas = globalPessoas.filter(p => p.id !== id);
        resolve(); // Retorna void
      }, 500);
    }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pessoas'] });
    },
  });

  return {
    data: query.data,
    isLoading: query.isLoading,
    isError: query.isError,
    create: createMutation,
    update: updateMutation,
    remove: removeMutation,
  };
};

// --- FIM DOS MOCKS ---


const PessoasList = ({ pessoas, setEditing, remove }) => (
    <div className="flex flex-col gap-3">
        {pessoas?.map((p) => (
            <div
                key={p.id}
                // Dark Mode: Card de fundo cinza escuro, sombra clara e hover sutil
                className="bg-gray-800 p-4 rounded-lg shadow-lg flex justify-between items-center transition duration-200 hover:shadow-xl hover:bg-gray-700 cursor-pointer border border-gray-700"
            >
                <div className="flex flex-col">
                    <h2 className="font-semibold text-lg text-white">{p.nome}</h2>
                    <p className="text-gray-400 text-sm mt-0.5">Nascimento: {p.dataNascimento}</p>
                </div>
                <div className="flex gap-1">
                    <button
                        // Dark Mode: Hover azul escuro
                        className="p-2 text-blue-400 hover:bg-blue-900 rounded-full transition duration-150"
                        onClick={() => {
                            setEditing(p);
                        }}
                        aria-label="Editar Pessoa"
                    >
                        {/* Ícone de Editar (Pencil Outline) - Mais Flat */}
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                        </svg>
                    </button>
                    <button
                        // Dark Mode: Hover vermelho escuro
                        className="p-2 text-red-400 hover:bg-red-900 rounded-full transition duration-150"
                        onClick={() => remove.mutate(p.id)}
                        aria-label="Excluir Pessoa"
                    >
                        {/* Ícone de Excluir (Trash Outline) - Mais Flat */}
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                        </svg>
                    </button>
                </div>
            </div>
        ))}
    </div>
);


const PessoaFormModal = ({ editing, setEditing, setFormVisible, nome, setNome, dataNascimento, setDataNascimento, handleSave }) => (
    <div className="fixed inset-0 flex items-center justify-center bg-black/70 z-50 p-4 transition-opacity duration-300">
        <div 
            // Dark Mode: Modal com fundo escuro
            className="bg-gray-800 p-6 rounded-lg shadow-2xl w-full max-w-md flex flex-col gap-5 transform transition-all duration-300 border border-gray-700"
        >
            <h2 className="text-2xl font-bold text-white border-b border-gray-700 pb-3">
                {editing ? 'Editar Pessoa' : 'Nova Pessoa'}
            </h2>
            
            <input
                // Dark Mode: Input com fundo cinza mais claro e texto branco
                className="border-b-2 border-gray-600 p-3 pt-4 rounded-t-lg focus:outline-none focus:border-blue-500 transition duration-150 text-white bg-gray-700 hover:bg-gray-600"
                placeholder="Nome Completo"
                value={nome}
                onChange={(e) => setNome(e.target.value)}
            />
            
            <input
                type="date"
                // Dark Mode: Input com fundo cinza mais claro e texto branco
                className="border-b-2 border-gray-600 p-3 pt-4 rounded-t-lg focus:outline-none focus:border-blue-500 transition duration-150 text-white bg-gray-700 hover:bg-gray-600"
                value={dataNascimento}
                onChange={(e) => setDataNascimento(e.target.value)}
            />
            
            <div className="flex justify-end gap-3 pt-4">
                <button
                    onClick={() => {
                        setFormVisible(false);
                        setEditing(null);
                        setNome('');
                        setDataNascimento('');
                    }}
                    // Dark Mode: Botão de texto mais claro com hover escuro
                    className="text-gray-300 px-4 py-2 font-medium hover:bg-gray-700 rounded transition duration-150"
                >
                    Cancelar
                </button>
                <button
                    onClick={handleSave}
                    // Botão principal em azul, funciona bem no dark mode
                    className="bg-blue-600 text-white px-4 py-2 rounded font-medium hover:bg-blue-700 transition duration-150 shadow-md"
                >
                    {editing ? 'Salvar' : 'Criar'}
                </button>
            </div>
        </div>
    </div>
);


const App = () => {
    // Usamos o mock do hook
    const { data: pessoas, isLoading, isError, create, update, remove } = usePessoas();
    
    /** @type {[Pessoa | null, React.Dispatch<React.SetStateAction<Pessoa | null>>]} */
    const [editing, setEditing] = useState(null);
    const [formVisible, setFormVisible] = useState(false);
    const [nome, setNome] = useState('');
    const [dataNascimento, setDataNascimento] = useState('');

    // Preenche o formulário quando 'editing' muda
    React.useEffect(() => {
        if (editing) {
            setNome(editing.nome);
            setDataNascimento(editing.dataNascimento);
            setFormVisible(true);
        }
    }, [editing]);

    const handleSave = () => {
        if (!nome || !dataNascimento) return;

        if (editing) {
            update.mutate({ id: editing.id, pessoa: { nome, dataNascimento } });
        } else {
            create.mutate({ nome, dataNascimento });
        }
        
        // Reset do estado
        setFormVisible(false);
        setEditing(null);
        setNome('');
        setDataNascimento('');
    };

    return (
        // Dark Mode: Fundo principal preto/cinza muito escuro
        <div className="min-h-screen bg-gray-900 flex flex-col items-center p-4 font-sans">
            
            {/* Título e Botão Centralizados (Dark Mode Card) */}
            <div 
                // Dark Mode: Card de fundo escuro, texto branco
                className="w-full max-w-xl bg-gray-800 p-6 rounded-lg shadow-xl mt-8 mb-6 border border-gray-700"
            >
                <div className="flex justify-between items-center">
                    <h1 className="text-3xl font-bold text-white">Gestão de Pessoas</h1>
                    <button
                        onClick={() => { setFormVisible(true); setEditing(null); setNome(''); setDataNascimento(''); }}
                        // Botão principal em azul, funciona bem no dark mode
                        className="bg-blue-600 text-white px-4 py-2 rounded-lg font-medium shadow-md hover:bg-blue-700 transition duration-150 flex items-center gap-2"
                        disabled={create.isLoading || update.isLoading || remove.isLoading}
                    >
                        {/* Ícone de Novo (Plus Outline) - Mais Flat */}
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M12 4v16m8-8H4" />
                        </svg>
                        Novo
                    </button>
                </div>
            </div>

            {/* Container Principal */}
            <div className="w-full max-w-xl">
                {/* Indicadores de Status (Dark Mode) */}
                {(isLoading || create.isLoading || update.isLoading || remove.isLoading) && (
                    <div className="text-center p-3 rounded-lg bg-blue-900 text-blue-300 font-medium shadow-sm mb-4">
                        {isLoading && "Carregando Pessoas..."}
                        {(create.isLoading || update.isLoading || remove.isLoading) && "Processando alteração..."}
                    </div>
                )}
                
                {isError && (
                    <div className="text-center p-3 rounded-lg bg-red-900 text-red-300 font-medium shadow-sm mb-4">
                        Erro ao carregar pessoas
                    </div>
                )}

                <PessoasList pessoas={pessoas} setEditing={setEditing} remove={remove} />
            </div>

            {/* Modal de formulário */}
            {formVisible && (
                <PessoaFormModal 
                    editing={editing}
                    setEditing={setEditing}
                    setFormVisible={setFormVisible}
                    nome={nome}
                    setNome={setNome}
                    dataNascimento={dataNascimento}
                    setDataNascimento={setDataNascimento}
                    handleSave={handleSave}
                />
            )}
        </div>
    );
};

const PessoasApp = () => (
    <QueryClientProvider client={queryClient}>
        <App />
    </QueryClientProvider>
);

export default PessoasApp;
