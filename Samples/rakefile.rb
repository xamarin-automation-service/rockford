require "benchmark"
require "date"

task :default => ['build:android', 'build:ios']

namespace :build do
  desc "Cleans the project"
  task :clean do
    directories_to_delete = [
        "Ios/cs/SimpleApp/bin/iPhone",
        "Ios/cs/SimpleApp/bin/iPhoneSimulator",
        "Ios/cs/SimpleApp/bin//obj",
        "Android/cs/SimpleApp/SimpleApp/bin/Debug",
        "Android/cs/SimpleApp/SimpleApp/bin/Release",
        "Android/cs/SimpleApp/SimpleApp/obj"
    ]

    directories_to_delete.each { |x|
      rm_rf x
    }
  end

  desc "Build iOS App"
  task :ios => [:clean] do
    sh "pwd"
    sh "nuget restore Ios/cs/SimpleApp/packages.config -PackagesDirectory packages/"

    # sh "nuget restore ../../src/MobileApp/XamarinCRM.UITest/packages.config -PackagesDirectory ../../src/MobileApp/packages/"
    puts "iOS nugets restored"
    puts
    puts "Building XamarinCRM.iOS"

      time = time_cmd "xbuild Ios/cs/SimpleApp/SimpleApp.sln /p:Configuration='Debug' /p:Platform=iPhone /p:BuildIpa=true /verbosity:quiet /flp:LogFile=build_ios.log"

    # puts "Checking for Insights on iOS"
    # sh "cat ../../src/MobileApp/XamarinCRM.iOS/Main.cs | grep Insights | grep 2b82ddc37582e6c1bece7e5901e8bae3bf7bfb26"

      size = (File.size("Ios/cs/SimpleApp/bin/iPhone/Debug/SimpleApp-1.ipa")/1000000.0).round(1)
      log_data "iOS", time, size, "build_ios.log"

    puts "iOS BUILD SUCCESSFUL"
  end

  desc "Build Android App"
  task :android => [:clean] do
    sh "nuget restore Android/cs/SimpleApp/SimpleApp/packages.config -PackagesDirectory ../packages"
    sh "nuget restore Android/cs/SimpleApp/Library.Client/packages.config -PackagesDirectory ../packages/"
    sh "nuget restore Android/cs/SimpleApp/Library.Server/packages.config -PackagesDirectory ../packages/"
    puts "Android nugets restored"
    puts
    # addMaptoManifest("../../src/MobileApp/XamarinCRM.Android/Properties/AndroidManifest.xml")
    # puts "Maps added to manifest"
    puts "Building XamarinCRM.Android"

    time = time_cmd "xbuild Android/cs/SimpleApp/SimpleApp/SimpleApp.csproj /p:Configuration=Debug /t:SignAndroidPackage /verbosity:quiet /flp:LogFile=build_android.log"

    # puts "Checking for Insights on Android"
    # sh "cat ../../src/MobileApp/XamarinCRM.Android/MainActivity.cs | grep Insights | grep 2b82ddc37582e6c1bece7e5901e8bae3bf7bfb26"

      size = (File.size("Android/cs/SimpleApp/SimpleApp/bin/Debug/SimpleApp.SimpleApp-Signed.apk")/1000000.0).round(1)
      log_data "Android", time, size, "build_android.log"

    puts "Android BUILD SUCCESSFUL"
  end

  def time_cmd(cmd)
    time = Benchmark.realtime do
      sh cmd
    end
    min = (time / 60).to_i.to_s
    sec = (time % 60).to_i.to_s
    sec = sec.length < 2 ? "0" + sec : sec
    return "#{min}:#{sec}"
  end

  def log_data(platform, time, size, log_file)
    date = DateTime.now.strftime("%m/%d/%Y %I:%M%p")
    version = /\d+\.\d+\.\d+\.\d+/.match(`mdls -name kMDItemVersion /Applications/Xamarin\\ Studio.app`)
    user = /\w+$/.match(ENV['HOME'])[0].capitalize

    tail = `tail -n 6 #{log_file}`
    warnings = /(\d+) Warning\(s\)/.match(tail).captures[0]
    errors = /(\d+) Error\(s\)/.match(tail).captures[0]

    puts "*** origin: #{user}"
      puts "*** xamarin version: #{version}"
      puts "*** platform: #{platform}"
      puts "*** date time: #{date}"
      puts
      puts "*** build time: #{time}"
      puts "*** app size (MB): #{size}"
      puts "*** warnings: #{warnings}"
      puts "*** errors: #{errors}"
  end
end
