import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import type { Pessoa } from '../models/pessoa'
import { pessoaApi } from '../apis/pessoaApi'

export function usePessoas() {
  const queryClient = useQueryClient()

  const onSuccess = () => queryClient.invalidateQueries({ queryKey: ['pessoas'] })

  const query = useQuery<Pessoa[], Error>({ queryKey: ['pessoas'], queryFn: pessoaApi.search });

  const create = useMutation<Pessoa, Error, Omit<Pessoa, 'id'>>({
    mutationFn: pessoaApi.create, onSuccess 
  })

  const update = useMutation<Pessoa, Error, { id: string; pessoa: Partial<Pessoa> }>({
    mutationFn: ({ id, pessoa }) => pessoaApi.update(id, pessoa), onSuccess
  })

  const remove = useMutation<any, Error, string>({ mutationFn: pessoaApi.delete, onSuccess })

  return { ...query, create, update, remove };
};
