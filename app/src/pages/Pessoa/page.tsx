import type { Pessoa } from "../../models/pessoa"
import { PessoaTable } from "./table"
import { PessoaForm } from "./form"
import './pessoa.css'

interface Props {
   src: Pessoa[]
}

export const PessoaPage = (props: Props) => <>
   <section id="pessoa">      
      <aside id="panel">
         Filtrar: <input />
         <button>Adicionar</button>
         <br/>
      </aside>
      <section>
         {/* <PessoaForm /> */}
         <PessoaTable src={props.src} />
      </section>
   </section>
</>