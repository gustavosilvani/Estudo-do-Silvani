using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Api.Models;

/// <summary>
/// HATEOAS Link - Hypermedia as the Engine of Application State
/// Nível 3 do Richardson Maturity Model
/// </summary>
public class Link {
    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string? Type { get; set; }
    
    public Link() { }
    
    public Link(string href, string rel, string method = "GET") {
        Href = href;
        Rel = rel;
        Method = method;
    }
}

/// <summary>
/// Recurso base com HATEOAS
/// </summary>
public class ResourceBase {
    public List<Link> Links { get; set; } = new();
    
    public void AddLink(string href, string rel, string method = "GET") {
        Links.Add(new Link(href, rel, method));
    }
}

/// <summary>
/// DTO de Tarefa com HATEOAS (Nível 3 REST)
/// </summary>
public class TarefaHateoasDto : ResourceBase {
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Prioridade { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime? DataVencimento { get; set; }
    public bool EstaVencida { get; set; }
    public int TotalComentarios { get; set; }
}

/// <summary>
/// Builder para criar links HATEOAS
/// </summary>
public static class HateoasLinkBuilder {
    public static void AddTarefaLinks(
        this TarefaHateoasDto tarefa,
        IUrlHelper urlHelper) {
        
        // Self link (sempre presente)
        tarefa.AddLink(
            urlHelper.Action("BuscarPorId", "Tarefas", new { id = tarefa.Id })!,
            "self",
            "GET"
        );
        
        // Links baseados no estado da tarefa
        switch (tarefa.Status) {
            case "Pendente":
                // Pode iniciar, cancelar ou deletar
                tarefa.AddLink(
                    urlHelper.Action("IniciarExecucao", "Tarefas", new { id = tarefa.Id })!,
                    "iniciar",
                    "PATCH"
                );
                tarefa.AddLink(
                    urlHelper.Action("Cancelar", "Tarefas", new { id = tarefa.Id })!,
                    "cancelar",
                    "PATCH"
                );
                tarefa.AddLink(
                    urlHelper.Action("Deletar", "Tarefas", new { id = tarefa.Id })!,
                    "delete",
                    "DELETE"
                );
                break;
                
            case "EmAndamento":
                // Pode concluir ou cancelar
                tarefa.AddLink(
                    urlHelper.Action("Concluir", "Tarefas", new { id = tarefa.Id })!,
                    "concluir",
                    "PATCH"
                );
                tarefa.AddLink(
                    urlHelper.Action("Cancelar", "Tarefas", new { id = tarefa.Id })!,
                    "cancelar",
                    "PATCH"
                );
                break;
                
            case "Concluida":
            case "Cancelada":
                // Apenas pode deletar
                tarefa.AddLink(
                    urlHelper.Action("Deletar", "Tarefas", new { id = tarefa.Id })!,
                    "delete",
                    "DELETE"
                );
                break;
        }
        
        // Comentários sempre disponíveis (exceto se cancelada/concluída)
        if (tarefa.Status != "Concluida" && tarefa.Status != "Cancelada") {
            tarefa.AddLink(
                urlHelper.Action("AdicionarComentario", "Tarefas", new { id = tarefa.Id })!,
                "comentarios",
                "POST"
            );
        }
        
        // Link para listar todas
        tarefa.AddLink(
            urlHelper.Action("ListarTodas", "Tarefas")!,
            "todas-tarefas",
            "GET"
        );
    }
    
    public static void AddCollectionLinks(
        this List<TarefaHateoasDto> tarefas,
        IUrlHelper urlHelper) {
        
        foreach (var tarefa in tarefas) {
            tarefa.AddTarefaLinks(urlHelper);
        }
    }
}
