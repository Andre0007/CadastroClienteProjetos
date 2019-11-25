import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  uri = 'https://localhost:44311/api/Home';

  constructor(private http: HttpClient) { }

  getHome() {
    return this.http.get(`${this.uri}`);
  }

}
