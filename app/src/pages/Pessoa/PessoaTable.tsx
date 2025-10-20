import { css } from "@emotion/react";
import { useState } from "react";
import { Icon } from "../../components";
import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";

interface Props {
	onEdit: (e: Pessoa) => void;
}

export function PessoaTable(props: Props) {
	const [pagina, setPagina] = useState(1);
	const [linhas, setLinhas] = useState(10);
	const swr = usePessoa({ page: { length: linhas, number: pagina } });
	const pessoas = swr.value;

	if (swr.error) return <div>{swr.error.message}</div>;
	if (swr.await) return <div>carregando...</div>;

	function onSetPageSize(e) {
		const value = parseInt(e.target.value, 10) || 10;
		setLinhas(value);
	}

	const populando = (pessoas?.records || []).map((pessoa, i) => ({ props, pessoa, swr, i }));

	const paginacao = Array.range(1, pessoas?.pages ?? 1).map((now) => ({
		page: pessoas?.number ?? 1,
		apply: setPagina,
		now,
	}));

	return (
		<>
			<table id="pessoa-table">
				<thead>
					<tr>
						<th>Id</th>
						<th>Nome</th>
						<th>CPF</th>
						<th>Email</th>
						<th>Nascimento</th>
						<td>País</td>
						<th>Ação</th>
					</tr>
				</thead>
				<tbody>{populando.map(PessoaBody)}</tbody>
			</table>
			<section css={pageStyle} cols="auto 1fr 1fr">
				<select onChange={onSetPageSize} value={linhas}>
					<option value="2">2</option>
					<option value="10">10</option>
					<option value="25">25</option>
					<option value="50">50</option>
					<option value="75">75</option>
					<option value="100">100</option>
				</select>
				<div className="page">{paginacao.map(PageNumber)}</div>
				<div className="page counter">{pessoas?.sum} total de registros</div>
			</section>
		</>
	);
}

const PageNumber = ({ apply, page, now }: { apply: Function; page: number; now: number }) => (
	<span key={page}>
		{page === now && <span>{page}</span>}
		{page !== now && (
			<a role="none" onClick={() => apply(page)}>
				{page}
			</a>
		)}
	</span>
);

function PessoaBody({ props, pessoa, swr, i }) {
	const convertDataNascimento = (p: Pessoa) => Date.fromToString(p.nascimento, "yyyy-MM-dd", "dd/MM/yyyy");

	return (
		<tr key={i}>
			<td>{pessoa.id?.slice(0, 7)}</td>
			<td>{pessoa.nome}</td>
			<td>{pessoa.cpf}</td>
			<td>{pessoa.email}</td>
			<td>{convertDataNascimento(pessoa)}</td>
			<td>{pessoa.nacionalidade}</td>
			<td style={{ textAlign: "center" }}>
				<Icon tooltip="editar" name="edit_note" onClick={() => props.onEdit(pessoa)} />
				<Icon tooltip="deletar" name="delete" onClick={() => swr.drop(pessoa.id)} />
			</td>
		</tr>
	);
}

const pageStyle = css`	
	div.page.counter {
		text-align: right;
		justify-self: end;
	}

	div.page {
		margin-left: 15px;
	}

	div.page > span {
		font-weight: bolder;
		margin-top: 5px;
	}

	div.page > span > a,
	div.page > span > a:hover,
	div.page > span > span {
		display: inline-block;
		padding: 0px 10px !important;
		border: solid 1px #444;
		line-height: 35px;
	}

	div.page > span > span {
		color: var(--inverse);
		background: var(--primary);
		font-weight: bolder;
	}
`;
