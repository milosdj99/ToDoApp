import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({providedIn: 'root'})
export class AuthGuardService implements CanActivate {

  constructor(public auth: AuthService, public router: Router) {}
  
  canActivate(): boolean {
    let isAuthenticated;
    this.auth.isAuthenticated$.subscribe( data => { isAuthenticated = data;})
    if (!isAuthenticated) {
      this.router.navigate(['dashboard']);
      return false;
    }
    return true;
  }
}