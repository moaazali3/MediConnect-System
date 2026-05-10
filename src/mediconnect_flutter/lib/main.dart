import 'package:flutter/material.dart';
import 'package:mediconnect/auth/screens/login_screen.dart';
import 'package:mediconnect/admin/admin_dashboard.dart';
import 'package:mediconnect/home_screen.dart';
import 'package:mediconnect/Doctor/doctor_home_screen.dart';
import 'package:mediconnect/receptionist/receptionist_dashboard.dart';
import 'package:mediconnect/services/secure_storage.dart';
import 'package:mediconnect/services/api_service.dart';
import 'package:mediconnect/services/theme_service.dart';
import 'package:mediconnect/constants/app_theme.dart';
import 'package:shared_preferences/shared_preferences.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await SecureStorage.init();
  await ThemeService().init();

  // تحميل التوكن المحفوظ في الذاكرة للطلبات القادمة
  String? token = await SecureStorage.readData(key: 'auth_token');
  ApiService.setToken(token);

  final prefs = await SharedPreferences.getInstance();
  String? role = prefs.getString('user_role');
  String? userId = prefs.getString('user_id');

  Widget homeWidget = const LoginScreen();

  if (token != null && role != null && userId != null) {
    // ── Validate the token by attempting a refresh ──
    // If the refresh token is also expired, we clear everything and send the
    // user to the login screen rather than letting them reach a dashboard with
    // an invalid session.
    final refreshResult = await ApiService().refreshToken();

    if (refreshResult.success) {
      // Token renewed — route to the correct dashboard
      final String lowerRole = role.toLowerCase();
      if (lowerRole == "admin") {
        homeWidget = const AdminDashboard();
      } else if (lowerRole == "doctor") {
        homeWidget = DoctorHomeScreen(userId: userId);
      } else if (lowerRole == "receptionist") {
        homeWidget = const ReceptionistDashboard();
      } else {
        homeWidget = HomeScreen(userId: userId, userRole: role);
      }
    } else {
      // Token expired and refresh failed — wipe session and go to Login
      await SecureStorage.deleteAllData();
      ApiService.setToken(null);
      await prefs.clear();
      homeWidget = const LoginScreen();
    }
  }

  runApp(MyApp(homeWidget: homeWidget));
}

class MyApp extends StatelessWidget {
  final Widget homeWidget;
  const MyApp({super.key, required this.homeWidget});

  @override
  Widget build(BuildContext context) {
    // AnimatedBuilder rebuilds the whole tree when ThemeService notifies.
    return AnimatedBuilder(
      animation: ThemeService(),
      builder: (context, _) {
        return MaterialApp(
          navigatorKey: ApiService.navigatorKey,
          debugShowCheckedModeBanner: false,
          title: 'MediConnect',
          theme: AppTheme.light,
          darkTheme: AppTheme.dark,
          themeMode: ThemeService().themeMode,
          home: homeWidget,
        );
      },
    );
  }
}
