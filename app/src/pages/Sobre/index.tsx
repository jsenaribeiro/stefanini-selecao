export default function Sobre() {
	return (
		<>
			<h1>Sobre</h1>
			<p>
				Este projeto é um sistema simples de gerenciamento de pessoas, desenvolvido com foco em
				produtividade e facilidade de uso. Ele permite criar, visualizar, atualizar e excluir registros de
				forma rápida e intuitiva. A aplicação utiliza tecnologias modernas do ecossistema React, garantindo
				desempenho e responsividade. Além disso, a estrutura do código foi pensada para escalabilidade e
				manutenção futura. O objetivo principal é fornecer uma solução prática para o controle de
				informações de pessoas em diferentes contextos.
			</p>
			<style>{style}</style>
		</>
	);
}

const style = `
	p {
		margin-top: 20px;
	}
`;
