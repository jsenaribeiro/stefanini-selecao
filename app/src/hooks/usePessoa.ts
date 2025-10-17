import { useQuery, useMutation, useQueryClient, type UseMutationResult, type UseQueryResult, QueryClient } from '@tanstack/react-query'
import type { Pessoa } from '../models/pessoa'
import { pessoaApi } from '../apis/pessoaApi'

export function usePessoas() {
  const queryClient = useQueryClient()

  const onSuccess = () => queryClient.invalidateQueries({ queryKey: ['pessoas'] })

  const search = useQuery<Pessoa[], Error>({ queryKey: ['pessoas'], queryFn: pessoaApi.search });

  const create = useMutation<Pessoa, Error, Omit<Pessoa, 'id'>>({
    mutationFn: pessoaApi.create, onSuccess
  })

  const update = useMutation<Pessoa, Error, { pessoa: Partial<Pessoa> & { id: string} }>({
    mutationFn: ({ id, pessoa }) => pessoaApi.update(id, pessoa), onSuccess
  })

  const remove = useMutation<any, Error, string>({ mutationFn: pessoaApi.delete, onSuccess })


  // // return CRUD<Pessoa>.from(queryClient, ['pessoas'])
  // return new CRUD<Pessoa, string>({ search, create, update, remove });

  // const crud: ICRUD<Pessoa, Error> = {
    
  }
}
