import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  uri = 'https://localhost:44311/api/Cliente';

  constructor(private http: HttpClient) { }  

  addCliente(razaoSocial, cnpj) {
    const headers: HttpHeaders = new HttpHeaders();

    headers.set('Content-Type', 'application/json');

    const obj = {
      razaoSocial: razaoSocial,
      cnpj: cnpj
    };
    return this.http.post(`${this.uri}`, obj, { headers: headers })
  }

  getCliente() {
    return this.http.get(`${this.uri}`);
  }

  editCliente(id) {
    return this.http.get(`${this.uri}/${id}`);
  }

  updateCliente(razaoSocial, cnpj, id) {
    const obj = {
      razaoSocial: razaoSocial,
      cnpj: cnpj,
      id: id
    };
    return this.http.put(`${this.uri}`, obj);     
  }

  deleteCliente(id) {
    return this.http.delete(`${this.uri}/${id}`);
  }

}
