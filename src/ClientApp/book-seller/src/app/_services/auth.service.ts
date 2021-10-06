import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const AUTH_API = 'http://localhost:5113/api/1/account/';
//const AUTH_API_Ac = 'http://localhost:5111/api/1/';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any> {
    let body = new URLSearchParams();
    body.set('userName', email);
    body.set('password', password);
    body.set('grant_type', 'password');
    body.set('client_id', 'angular-client');
    return this.http.post(AUTH_API + 'token', body.toString(), httpOptions);
  }

  register(firstName: string, lastName:string, email: string, password: string): Observable<any> {
    return this.http.post(AUTH_API + 'register', {
      firstName,
      lastName,
      email,
      password
    });
  }
}
