var path = require('path');
var webpack = require('webpack')

module.exports = {
    mode: "development",
    entry: {
        Test: './wwwroot/js/Test/Entry.ts',
    },
    devtool: 'inline-source-map',
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot/prod/js/pages/Test')
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