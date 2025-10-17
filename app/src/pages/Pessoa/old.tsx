
import { usePessoas } from '../../hooks/usePessoa'
import type { Pessoa } from '../../models/pessoa'
import { useReactive } from '../../hooks/useReactive'

interface PageModel {
  editing: Pessoa | null
  dataNascimento: string
  formVisible: boolean
  nome: string
}

interface Props {
  pessoa: Pessoa
  model: PageModel
  del: (id: any) => any
}

export function PessoasPage() {
  const { data: pessoas, isLoading, isError, create, update, remove } = usePessoas()
  
  const model = useReactive<PageModel>({
    editing: null,
    dataNascimento: '',
    formVisible: false,
    nome: ''
  })

  function handleSave() {
    const { nome, dataNascimento } = model

    if (!model.editing) create.mutate({ nome, dataNascimento })

    else update.mutate({ id: model.editing.id, pessoa: { nome, dataNascimento } })

    model.formVisible = false
    model.dataNascimento = ''
    model.editing = null
    model.nome = ''
  }

  const setVisible = (show: boolean) => { model.formVisible = show }

  const createProps = (pessoa: Pessoa) => ({ pessoa, model, del: remove.mutate })

  if (isLoading) return <div>Carregando...</div>
  if (isError) return <div>Erro ao carregar pessoas</div>

  return <>
    <center>
      <div>
        <h1>Pessoas</h1>
        <button onClick={() => setVisible(true)} >
          ➕ Nova
        </button>
      </div>

      {model.formVisible && (
        <section>
          <input
            placeholder="Nome"
            value={model.nome}
            onChange={e => model.nome = e.target.value} />
          
          <input
            type="date"
            value={model.dataNascimento}
            onChange={e => model.dataNascimento = e.target.value} />

          <div>
            <button onClick={handleSave}>
              {model.editing ? 'Atualizar' : 'Criar'}
            </button>
            <button
              onClick={() => (model.formVisible = false) || (model.editing = null) }>
              Cancelar
            </button>
          </div>
        </section>
      )}

      <section>{ pessoas?.map(createProps).map(PessoaItem) } </section>
    </center>
  </>
}

const PessoaItem = ({ pessoa, model, del }: Props) => <>
  <section key={pessoa.id}>
    <div>
      <h2>{pessoa.nome}</h2>
      <p>{pessoa.dataNascimento}</p>
    </div>
    <div>
      <button
        onClick={function() {
          model.editing = pessoa
          model.nome = pessoa.nome
          model.dataNascimento = pessoa.dataNascimento
          model.formVisible = true
        }} >
        ✏️
      </button>
      <button onClick={() => del(pessoa.id)}>
        🗑️
      </button>
    </div>
  </section>
</>

export default PessoasPage;