var path = require('path');
var webpack = require('webpack');
module.exports = {
    entry: {
        site: [
            './wwwroot/js/site.js',
        ]
    },
    output: {
        filename: '[name].min.js',
        publicPath: "/dist/",
        path: path.resolve(__dirname, 'wwwroot/dist'),
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: [
                    {
                        loader: 'ts-loader',
                        options: {
                            compilerOptions: {
                                "module": "commonjs",
                                "lib": ["dom", "es7"],
                                "noLib": false,
                            },
                            onlyCompileBundledFiles: true //loads only those files that are actually bundled by webpack
                        }
                    }
                ],
                exclude: /node_modules/,
            },
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js', '.css'],
    }
};
//# sourceMappingURL=webpack.config.js.map