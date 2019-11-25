import { Component, OnInit } from '@angular/core';
import Projeto from '../../domain/Projeto';
import { ProjetoService } from '../../service/projeto.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { GenericValidator } from '../../Util/GenericValidator';
import Swal from 'sweetalert2';
import * as $ from 'jquery';
declare var $: any;

@Component({
  selector: 'app-cliente-crud',
  templateUrl: './projeto-component.html',
  styleUrls: ['./projeto-component.css']
})
export class ProjetoComponent implements OnInit {

  projetos: Projeto[];
  projeto: any = {};
  angForm: FormGroup;
  public paginaAtual = 1;

  constructor(private fb: FormBuilder,
    private bs: ProjetoService) {
    this.createForm();
  }

  ngOnInit() {
    this.bs
      .getProjeto()
      .subscribe((data: Projeto[]) => {
        this.projeto = data;
      });
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
    this.projeto.nomeProjeto = '';
    this.projeto.idCliente = '';
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
    this.bs.editProjeto(id).subscribe((data: Projeto) => {
      this.projeto = data;
    });
  }

  updateCliente(razaoSocial, cnpj) {
    this.bs.updateProjeto(razaoSocial, cnpj, this.projeto.id).subscribe(
      (next: Response) => {
        this.CloseModal('Atualizado', next), this.ngOnInit()
      },
      (erro: Response) => {
        this.CloseModal('Atualizar', erro)
      });;
  }

  deleteCliente(id) {
    this.bs.deleteProjeto(id).subscribe(
      (next: Response) => {
        this.CloseModal('Deletado', next), this.ngOnInit()
      },
      (erro: Response) => {
        this.CloseModal('Deletar', erro)
      });
  }

}
