# Generate domain entities from database (ef core)


dotnet ef dbcontext scaffold `
	"data source=localhost;database=roadie;integrated security=true;MultipleActiveResultSets=True" `
	Microsoft.EntityFrameworkCore.SqlServer `
	--context RoadieEntities `
	--force `
	--startup-project Sigma.Roadie.Server `
	--project Sigma.Roadie.Domain `
	--output-dir DataModels `
	--data-annotations

