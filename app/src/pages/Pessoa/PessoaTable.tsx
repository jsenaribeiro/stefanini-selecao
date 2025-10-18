import { Icon } from "../../components";
import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";

interface Props {
	onEdit: (e: Pessoa) => void;
}

export function PessoaTable(props: Props) {
	const crud = usePessoa();
	const { data, isLoading, error } = crud;
	const pessoas = data || [];

	if (isLoading) return <progress>carregando...</progress>;

	if (error) return <div>{error.message}</div>;

	const onSet = (pessoa) => () => props.onEdit(pessoa);

	const onDelete = (id: any) => () => crud.delete(id);

	const PessoaBody = (pessoa: Pessoa, i: number) => (
		<tr key={i}>
			<td>{pessoa.id}</td>
			<td>{pessoa.nome}</td>
			<td>{pessoa.cpf}</td>
			<td>{pessoa.email}</td>
			<td>{pessoa.nascimento}</td>
			<td>{pessoa.nacionalidade}</td>
			<td style={{ textAlign: "center" }}>
				<Icon tooltip="editar" name="edit_note" onClick={onSet(pessoa)} />
				<Icon tooltip="deletar" name="delete" onClick={onDelete(pessoa.id)} />
			</td>
		</tr>
	);

	return (
		<table id="pessoa-table">
			<thead>
				<tr>
					<th>Id</th>
					<th>Nome</th>
					<th>CPF</th>
					<th>Email</th>
					<th>Nascimento</th>
					<td>Nação</td>
					<th>Ação</th>
				</tr>
			</thead>
			<tbody>{pessoas.map(PessoaBody)}</tbody>
		</table>
	);
}
