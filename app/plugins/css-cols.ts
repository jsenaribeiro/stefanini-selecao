const REGEX_COLS = /<(\w+)([^>]*)\scols=(["'])(.*?)\3([^>]*)>/g;

export const cssColsGridLayout: any = {
	name: "cols-transform",
	enforce: "pre",
	transform(code: string, file: string) {
		const requireds = [".tsx", ".jsx"];
		const checkExtension = (ext: string) => file.endsWith(ext);
		if (!requireds.some(checkExtension)) return null;
		return code.replace(REGEX_COLS, applyColsTransform);
	},
};

const applyColsTransform = (_: string, tag: string, last: string, __: string, data: string, next: string) => {
	const style = `display: 'grid', gridTemplateColumns: '${data}'`;
	return `<${tag}${last} style={{ ${style} }} ${next}>`;
};
