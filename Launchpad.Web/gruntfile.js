module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            boostrap: {
                files: [
                    { 
                        expand: true,
                        cwd: 'node_modules/bootstrap/dist/css/',
                        src: ['bootstrap.css'], 
                        dest: 'dist/bootstrap/css/', 
                        filter: 'isFile'
                    },
                    {
                        expand: true,
                        cwd: 'app/',
                        src: ['app.css'],
                        dest: 'dist/css',
                        filter: 'isFile'
                    }
                ]
            }
        },
        concat: {
            options: {
                separator: ';', 
                sourceMap : true
  
            },
            dist: {
                src: ['app/app.module.js', 'app/app.config.js', 'app/**/*.js', '!app/**/*.spec.js'],
                dest: 'dist/<%= pkg.name %>.js'
            },
            vendor:{
                src: ['node_modules/angular/angular.js', 'node_modules/angular-ui-router/release/angular-ui-router.js'],
                dest: 'dist/vendor.js'
            }
        },
        uglify: {
            options: {
                banner: '/*! <%= pkg.name %> <%= grunt.template.today("dd-mm-yyyy") %> */\n',
                sourceMap: true,
                sourceMapIncludeSources : true,
                sourceMapIn : 'dist/<%= pkg.name %>.js.map'
            },
            dist: {
                files: {
                    'dist/<%= pkg.name %>.min.js': ['<%= concat.dist.dest %>']
                }
            }
        },
        qunit: {
            files: ['test/**/*.html']
        },
        jshint: {
            files: ['Gruntfile.js', 'app/**/*.js'],
            options: {
                // options here to override JSHint defaults
                globals: {
                    jQuery: true,
                    console: true,
                    module: true,
                    document: true,
                    
                }
            }
        },
        watch: {
            files: ['<%= jshint.files %>'],
            tasks: ['build']
        },
        karma: {
            unit: {
                configFile: 'karma.conf.js'
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-karma');

    grunt.registerTask('test', ['jshint', 'karma']);
    grunt.registerTask('build', ['copy', 'jshint', 'concat']);
    grunt.registerTask('default', ['build']);

};