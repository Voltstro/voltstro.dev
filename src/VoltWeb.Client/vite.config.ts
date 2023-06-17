import { defineConfig, UserConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig(({ mode }) => {

	//Base vite config
	const config: UserConfig = {
		build: {
			//No minify in dev builds, speeds shit up
			minify: false,
			emptyOutDir: true,
			rollupOptions: {
				input: {
					main: resolve(__dirname, 'src/main.ts'),
				},
				output: {
					dir: resolve('..', 'VoltWeb', 'wwwroot'),
					entryFileNames: () => 'js/[name].js',
					chunkFileNames: () => 'js/[name].[hash].js',
					assetFileNames: () => 'assets/[name][extname]',
					sourcemap: false,
				}
			}
		},
		css: {
			modules: {
				scopeBehaviour: 'global'
			}
		}
	};
    
	//In non-dev builds, we will use terser to minify everything
	if (mode != 'development') {
		console.log('Non-dev build...');
		config.build!.minify = true;
	}

	return config;
});
