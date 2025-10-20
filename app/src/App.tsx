import { css } from "@emotion/react";
import { Link, Route, Routes } from "react-router-dom";
import Inicial from "./pages/Inicial";
import Pessoa from "./pages/Pessoa";
import Sobre from "./pages/Sobre";
import "./App.css";

function App() {
	console.log(cssApp);
	return (
		<main id="app">
			<aside>
				<nav id="menu">
					<Link to="/">Inicial</Link>
					<Link to="/sobre">Sobre</Link>
					<Link to="/pessoas">Pessoas</Link>
				</nav>
				<section>
					<Routes>
						<Route path="/" element={<Inicial />} />
						<Route path="/sobre" element={<Sobre />} />
						<Route path="/pessoas" element={<Pessoa />} />
					</Routes>
				</section>
			</aside>
			<style>{cssApp.styles}</style>
		</main>
	);
}

const cssApp = css`
#app {
	width: 800px;
}

#app nav {
	float: right;
	display: flex;
	justify-items: center;

	& a {
		padding: 0 20px;
		border-left: solid 1px grey;
		font-size: 1.2rem;
	}

	& a:first-of-type {
		border: 0;
	}
}

#app {
	box-sizing: unset;
}
`;

export default App;
