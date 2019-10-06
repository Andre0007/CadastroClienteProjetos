import { Component, OnInit } from '@angular/core';
import Cliente from '../domain/Cliente';
import { ClienteService } from '../service/cliente.service';
import { FormGroup, FormBuilder, Validators, FormControl  } from '@angular/forms';
import { GenericValidator } from '../Util/GenericValidator';
import Swal from 'sweetalert2';
import * as $ from 'jquery';
declare var $: any;

@Component({
  selector: 'app-cliente-crud',
  templateUrl: './Cliente.html'
})
export class ClienteComponent implements OnInit {
  clientes: Cliente[];
  cliente: any = {};
  angForm: FormGroup;
  public paginaAtual = 1;

  constructor(private fb: FormBuilder,
    private bs: ClienteService) {
    this.createForm();
  }

  createForm() {
    this.angForm = this.fb.group({
      razaoSocial: ['', Validators.required],
      cnpj: new FormControl('', Validators.compose([
        Validators.required,
        GenericValidator.isValidCnpj()
      ]))
    });
  }

  ShowModal(id) {
    $('#myModal').modal('show');
    if (id === null) {
      $('.modal-title').text("Criar novo Usuário");
      $('#btn-update').hide();
      $('#btn-create').show();
      this.cleanForm();
    } else {
      $('.modal-title').text("Alterar Usuário");
      $('#btn-update').show();
      $('#btn-create').hide();
    }
  }

  CloseModal(tipo, res) {
    if (res.error != null) {
      Swal.fire({
        type: 'error',
        title: 'Ops erro!!',
        text: res.error + ' ao tentar ' + tipo
      })
    } else {
      $('#myModal').modal('hide');
      Swal.fire({
        type: 'success',
        title: 'Sucesso!!',
        text: 'Registro ' + tipo + ' com sucesso!!'
      })
    }
  }

  cleanForm() {
    this.cliente.razaoSocial = '';
    this.cliente.cnpj = '';
  }

  addCliente(razaoSocial, cnpj) {    
    this.bs.addCliente(razaoSocial, cnpj).subscribe(
      (next: Response) => {
        this.CloseModal('Inserido', next), this.ngOnInit()
      },
      (erro: Response) => {
        this.CloseModal('Inserir', erro);       
      });
  }

  getClienteById(id) {
    this.ShowModal(id);
    this.bs.editCliente(id).subscribe((data: Cliente) => {
      this.cliente = data;
    });
  }

  updateCliente(razaoSocial, cnpj) {
    this.bs.updateCliente(razaoSocial, cnpj, this.cliente.id).subscribe(
      (next: Response) => {
        this.CloseModal('Atualizado', next), this.ngOnInit()
      },
      (erro: Response) => {
        this.CloseModal('Atualizar', erro)
      });;
  }

  deleteCliente(id) {
    this.bs.deleteCliente(id).subscribe(
      (next: Response) => {
        this.CloseModal('Deletado', next), this.ngOnInit()
      },
      (erro: Response) => {
        this.CloseModal('Deletar', erro)
      });
  }

  ngOnInit() {
    this.bs
      .getCliente()
      .subscribe((data: Cliente[]) => {
        this.clientes = data;
      });    
  }

}
