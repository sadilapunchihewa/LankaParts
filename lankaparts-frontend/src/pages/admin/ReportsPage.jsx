import { FileText } from 'lucide-react'
import { EmptyState, PageHeader } from '../../components/common/AdminUi'

export default function ReportsPage() {
  return <section className="seller-page"><PageHeader eyebrow="Reports" title="Reports" description="Export-ready marketplace reporting placeholders for sales, seller performance, and catalog quality." /><EmptyState icon={FileText} title="Reports workspace ready" message="Add export endpoints here when backend reporting is available." /></section>
}
