import Pessoa from "./pages/Pessoa";
import "./App.css";
import { Link, Route, Routes } from "react-router-dom";
import Inicial from "./pages/Inicial";
import Sobre from "./pages/Sobre";

const App = () => (
	<>
		<main id="main">
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
		</main>
	</>
);

export default App;
