import { PessoaPage } from "./pages/Pessoa/page";
import './App.css'

const App = () => <>
	<main id="main">
		<aside>
			<nav id="menu">
				<a>Home</a>
				<a>Sobre</a>
				<a>Pessoas</a>
			</nav>
			<section>
				<h1> Pessoas </h1>
				<hr />
				<PessoaPage src={[]} />
			</section>
		</aside>
	</main>
</>

export default App





