import type { Pessoa } from "../../models/pessoa"

interface Props {
   src: Pessoa[]
}

const convertToHeader = (pessoas: Pessoa[]) => pessoas.length 
	&& Object.keys(pessoas?.at(0) ?? {})
		.map(n => n.capitalize())
		.map(PessoaHead)

export const PessoaTable = (props: Props) => <>
	<table id="pessoa-table">
		<thead>
			<tr>{ convertToHeader(props.src) }</tr>
		</thead>
		<tbody>{ props.src.map(PessoaBody) }</tbody>
	</table>
</>

const PessoaHead = (campo: string, i: any) => <th key={i}>{campo}</th>

const PessoaBody = (pessoa: Pessoa, i: number) => 
	<tr key={i}>{Object.values(pessoa).map(PessoaCell)}</tr>

const PessoaCell = (value: any, i: number) => <td key={i}>{value}</td>