import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProjetoService {

  uri = 'https://localhost:44311/api/Projeto';

  constructor(private http: HttpClient) { }

  addCliente(nomeProjeto, idCliente) {
    const headers: HttpHeaders = new HttpHeaders();
    headers.set('Content-Type', 'application/json');

    const obj = {
      nomeProjeto: nomeProjeto,
      idCliente: idCliente
    };
    return this.http.post(`${this.uri}`, obj, { headers: headers });
  }

  getProjeto() {
    return this.http.get(`${this.uri}`);
  }

  editProjeto(id) {
    return this.http.get(`${this.uri}/${id}`);
  }

  updateProjeto(nomeProjeto, idCliente, id) {
    const obj = {
      nomeProjeto: nomeProjeto,
      idCliente: idCliente
    };
    return this.http.post(`${this.uri}`, obj);
  }

  deleteProjeto(id) {
    return this.http.get(`${this.uri}/${id}`);
  }

}
