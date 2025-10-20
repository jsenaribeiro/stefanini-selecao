const REGEX_COLS = /<(\w+)([^>]*)\scols=(["'])(.*?)\3([^>]*)>/g;

export const cssColsGridLayout: any = {
	name: "cols-transform",
	enforce: "pre",
	transform(code, file) {
		const requireds = [".tsx", ".jsx"];
		const checkExtension = (ext) => file.endsWith(ext);

		if (!requireds.some(checkExtension)) return null;
		return code.replace(REGEX_COLS, applyColsTransform);
	},
};

const applyColsTransform = (_, tag, last, __, data, next) => {
	console.log({ _, tag, last, __, data, next });

	const style = `display: 'grid', gridTemplateColumns: '${data}'`;
	return `<${tag}${last} style={{ ${style} }} ${next}>`;
};
