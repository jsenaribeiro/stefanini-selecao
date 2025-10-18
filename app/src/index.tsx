import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter } from "react-router-dom";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./assets/dev.css";
import "./assets/index.css";
import "./extensions";

const queryClient = new QueryClient();

const rootHTML = document.getElementById("root");

ReactDOM.createRoot(rootHTML!).render(
	<React.StrictMode>
		<BrowserRouter>
			<QueryClientProvider client={queryClient}>
				<App />
			</QueryClientProvider>
		</BrowserRouter>
	</React.StrictMode>,
);
