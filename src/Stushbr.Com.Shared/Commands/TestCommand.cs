using MediatR;
using Stushbr.Com.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stushbr.Com.Shared.Commands;
public sealed record TestCommand(string Blah) : ICommand<string>;
