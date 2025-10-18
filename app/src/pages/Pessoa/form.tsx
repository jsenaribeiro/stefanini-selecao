import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";
import type { PageModel } from "./types";

interface Props {
	model?: PageModel;
	onHide: () => {};
}

export function PessoaForm(props: Props) {
	const { create, update } = usePessoa();
	const pessoa = props.model?.item;
	const { id, nome, cpf, sexo, email, nacionalidade } = pessoa || {};

	const dataInvalida = !pessoa?.nascimento;
	const dataNascimento = Date.fromString(pessoa?.nascimento, "dd/MM/yyyy");
	const data = dataInvalida ? "" : dataNascimento.toString("yyyy-MM-dd");

	function onSubmit(e: React.FormEvent<HTMLFormElement>) {
		e.preventDefault();
		e.stopPropagation();

		const formData = new FormData(e.currentTarget);
		const pessoa = Object.fromEntries(formData.entries()) as any as Pessoa;
		const data = pessoa.nascimento as any;
		const nascimento =
			data instanceof Date
				? (data.toString("dd/MM/yyyy") as any)
				: Date.is(data, "yyyy-MM-dd")
					? Date.fromToString(data, "yyyy-MM-dd", "dd/MM/yyyy")
					: data;

		const action = props.model?.item ? update : create;

		pessoa.nascimento = nascimento;

		action(pessoa);
		props.onHide();
	}

	return (
		<dialog open>
			<h2>Cadastrar pessoa</h2>
			<form onSubmit={onSubmit}>
				<fieldset>
					<input hidden name="id" defaultValue={id || ""} />
					<section>
						<label htmlFor="nome">Nome</label>
						<input name="nome" defaultValue={nome} />
					</section>
					<section>
						<label htmlFor="cpf">CPF</label>
						<input name="cpf" defaultValue={cpf} />
					</section>
					<section>
						<label htmlFor="sexo">Sexo</label>
						<select name="sexo" defaultValue={sexo}>
							<option value="" disabled></option>
							<option value="M">Masculino</option>
							<option value="F">Feminino</option>
						</select>
					</section>
					<section>
						<label htmlFor="email">Email</label>
						<input name="email" type="email" defaultValue={email} />
					</section>
					<section>
						<label htmlFor="nacimento">Nascimento</label>
						<input name="nascimento" type="date" defaultValue={data} />
					</section>
					<section>
						<label htmlFor="nacionalidade">Nacionalidade</label>
						<input name="nacionalidade" defaultValue={nacionalidade} />
					</section>
				</fieldset>

				<section className="pessoa-botoes">
					<button type="reset" className="default" onClick={props.onHide}>
						Cancelar
					</button>
					<button type="submit" className="primary">
						Confirmar
					</button>
				</section>
			</form>
		</dialog>
	);
}
