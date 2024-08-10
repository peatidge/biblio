/// <binding AfterBuild='clean, sass:dist, uglify:dist' />
//TODO:b 229. Snort and Grunt through transpilation, uglification, ... to optimized enlightment
/*
 * https://nodejs.org/en
 * https://gruntjs.com/
 * https://rubyinstaller.org/
 * https://learn.microsoft.com/en-us/aspnet/core/client-side/using-grunt?view=aspnetcore-8.0
 * 1. Ensure Node.js & Ruby are installed (Ruby is required for SASS)
 * 2. Create a package.json file with devDependencies
 * 3. Restore npm packages (either cli or visual studio Dependencies npm > right click > Restore Packages)
 * 4. Add Grunt folder including Js and Sass subfolders
 */
//TODO:b 230. Configure Grunt tasks for JS and SASS
module.exports = function (grunt) {
    grunt.initConfig({
        clean: {
            js: {
                src: ["wwwroot/js/*.js", "wwwroot/js/*.map"],
            },
            sass: {
                src: "wwwroot/css/*.css",
            } 
        },          
        uglify: {
           
            options: {
                sourceMap: true,
                mangle: false,
                sourceMappingURL: function (path) {
                    return path.replace(/js\/build\/(.*).min.js/, "../../$1.map.js");
                },
                
            },
            dist: {
                files: [{
                    expand: true,
                    cwd: 'Grunt/Js',
                    src: '**/*.js',
                    dest: './wwwroot/js',
                    ext: '.min.js'
                   
                }]
            }
        },
        sass: {
            dist: {
                options: {
                    style: 'compressed'       
                },
                files: [{
                    expand: true,
                    cwd: 'Grunt/Sass',
                    src: ['*.scss'],
                    dest: './wwwroot/css',
                    ext: '.min.css'
                }]
            }
        },
        watch: {
            all: {
                files: ["Grunt/Js/*.js", "Grunt/Sass/*.scss"],
                tasks: ["uglify", "sass"] 
            },
            js: {
                files: ["Grunt/Js/*.js"],
                tasks: ["uglify"]
            },
            sass: {
                files: ["Grunt/Sass/*.scss"],
                tasks: ["sass"]
            },
                              
        },  
    });
    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.registerTask("gruntify",['clean:js','uglify','sass']);
   
};

//TODO:b 231-239. Read, Code, Play, Learn, Test & Repeat...
//https://learn.microsoft.com/en-us/aspnet/core/client-side/using-grunt?view=aspnetcore-8.0
//https://sass-lang.com/guide/
//https://getbootstrap.com/docs/5.2/getting-started/introduction/
//https://learn.jquery.com/
//https://fonts.google.com/icons
